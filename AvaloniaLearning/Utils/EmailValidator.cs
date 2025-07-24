namespace AvaloniaApp.Utils
{
    public static class EmailValidator
    {
        public static bool isEmailValid(string emailString)
        {
            if (string.IsNullOrWhiteSpace(emailString))
                return false;

            string[] spliter = emailString.Split('@');

            if (spliter.Length != 2)
                return false;

            string localPart = spliter[0];
            string domainPart = spliter[1];

            if (string.IsNullOrEmpty(localPart) || localPart.Length > 64)
                return false;

            foreach (char c in localPart)
            {
                if (!(char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '-' || c == '+'))
                    return false;
            }

            if (string.IsNullOrEmpty(domainPart))
                return false;

            string[] domainParts = domainPart.Split('.');

            if (domainParts.Length < 2)
                return false;

            foreach (string part in domainParts)
            {
                if (string.IsNullOrEmpty(part))
                    return false;

                if (part.Length > 63)
                    return false;

                if (part[0] == '-' || part[^1] == '-')
                    return false;

                foreach (char c in part)
                {
                    if (!(char.IsLetterOrDigit(c) || c == '-'))
                        return false;
                }
            }

            return true;
        }
    }
}
