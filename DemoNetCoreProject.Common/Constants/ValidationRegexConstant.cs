namespace DemoNetCoreProject.Common.Constants
{
    public class ValidationRegexConstant
    {
        public const string Ascii = "^[\x00-\x7F]*$";
        public const string Email = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
    }
}
