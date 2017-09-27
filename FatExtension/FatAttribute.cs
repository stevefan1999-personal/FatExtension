using System;

namespace FatExtension
{
    public class DescribeAttribute : Attribute
    {
        public DescribeAttribute(string _) { }
    }
    public class PurposeAttribute : Attribute
    {
        public PurposeAttribute(string _) { }
    }
    public class NoticeAttribute : Attribute
    {
        public NoticeAttribute(string _) { }
    }
    public class ExtraAttribute : Attribute
    {
        public ExtraAttribute(string _) { }
    }
    public class ThreadSafetyAttribute : Attribute
    {
        public ThreadSafetyAttribute(bool _) { }
    }
    public class WarningAttribute : Attribute
    {
        public WarningAttribute(string _) { }
    }
    public class CreatorAttribute : Attribute
    {
        public CreatorAttribute(params string[] _) { }
    }
    public class MaintainerAttribute : Attribute
    {
        public MaintainerAttribute(params string[] _) { }
    }
    public class ContributorAttribute : Attribute
    {
        public ContributorAttribute(params string[] _) { }
    }
}
