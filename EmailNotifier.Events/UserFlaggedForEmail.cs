using System;

namespace EmailNotifier.Events
{
    [Serializable]
    public class UserFlaggedForEmail
    {
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
