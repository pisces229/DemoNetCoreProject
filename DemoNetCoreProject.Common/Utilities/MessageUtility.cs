namespace DemoNetCoreProject.Common.Utilities
{
    public class MessageUtility
    {
        public const string NoMatchingData = "No Matching Data";
        public const string CreateSuccess = "Create Success";
        public const string ModifySuccess = "Modify Success";
        public const string RemoveSuccess = "Remove Success";
        public const string DataNotExist = "Data Not Exist";
        public const string DataExist = "Data Exist";
        public const string NotProvideData = "Not Provide Data";
        public static string DataExistWithName(string name) => $"【{name}】Exist";
    }
}
