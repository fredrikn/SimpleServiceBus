namespace ServiceBus.Infrastructure.Msmq
{
    using System;
    using System.Diagnostics;
    using System.Messaging;
    using Microsoft.Win32;

    internal class MsmqSetup
    {
        private const string ADMIN_SDDI = "S-1-5-32-544";
        private const string EVERYONE_SDDI = "S-1-1-0";
        private const string ANONOMOUS_SDDI = "S-1-5-7";

        public void EnsureQueueExists(string queueName)
        {
            try
            {
                EnsureQueueExistsImpl(queueName);
            }
            catch (Exception e)
            {
                throw new MessageQueueCreationException(
                    $"Unable to create the queue. Either create a private transactional queue with the name '{queueName}' or try to run the application as administrator.",
                    e);
            }
        }


        private static void EnsureQueueExistsImpl(string queueName)
        {
            var path = @".\Private$\" + queueName;
            if (!MessageQueue.Exists(path))
                MessageQueue.Create(path, true);

            TrySettingPermissions(path);

            //NOTE: The following register settings will make sure Load Balancer will work and
            //also make sure remote queues can be used. This section of code may be needed in the
            //cases a Load balancer is used or remote queues will be needed. 
            //NOTE: IgnoreOSNameValidation will disable the validation from where the message came.
            //SetRegisterValue(@"Software\Microsoft\MSMQ\Parameters", "IgnoreOSNameValidation", 1);

            //NOTE: AllowNonauthenticatedRPC will make sure a remote queue can be read from.
            //SetRegisterValue(@"Software\Microsoft\MSMQ\Parameters\Security", "AllowNonauthenticatedRPC", 1);
        }


        private static void TrySettingPermissions(string path)
        {
            try
            {
                var queue = new MessageQueue(path);
                queue.SetPermissions(GetIdentity(ADMIN_SDDI), MessageQueueAccessRights.FullControl);

                var everyone = GetIdentity(EVERYONE_SDDI);
                queue.SetPermissions(everyone, MessageQueueAccessRights.ReceiveMessage);
                queue.SetPermissions(everyone, MessageQueueAccessRights.PeekMessage);
                queue.SetPermissions(everyone, MessageQueueAccessRights.WriteMessage);
                queue.SetPermissions(everyone, MessageQueueAccessRights.GetQueueProperties);

                var anonomous = GetIdentity(ANONOMOUS_SDDI);
                queue.SetPermissions(anonomous, MessageQueueAccessRights.ReceiveMessage);
                queue.SetPermissions(anonomous, MessageQueueAccessRights.PeekMessage);
                queue.SetPermissions(anonomous, MessageQueueAccessRights.WriteMessage);
            }
            catch (Exception)
            {
                Debug.WriteLine("Unable to set permissions on the queue {0}.", path);
            }
        }



        //Used to set Register keyes for enamble the use of for example Load balancing or remote queues.
        //At the moment this is not used by default.
        private static void SetRegisterValue(string subKeyPath, string name, object value)
        {
            var subKey = Registry.LocalMachine.OpenSubKey(subKeyPath);

            if (subKey == null)
                subKey = Registry.LocalMachine.CreateSubKey(subKeyPath);

            subKey.SetValue(name, value, RegistryValueKind.DWord);
        }


        private static string GetIdentity(string sddi)
        {
            var securityIdentifier = new System.Security.Principal.SecurityIdentifier(sddi);
            var identityReference = securityIdentifier.Translate(typeof(System.Security.Principal.NTAccount));
            return identityReference.Value;
        }
    }
}