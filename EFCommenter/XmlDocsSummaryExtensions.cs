using Namotion.Reflection;

namespace EFCommenter;

internal static class XmlDocsSummaryExtensions
{
    public static string GetXmlSummary<T>(this T input)
    {
        var memberInfo = typeof(T).GetMember(input.ToString()).FirstOrDefault();
        if (memberInfo == null)
            return "";
        return memberInfo.GetXmlDocsSummary();
    }
    public static string GetXmlSummary(this Type type,string memberName)
    {
        var memberInfo = type.GetMember(memberName).FirstOrDefault();
        if (memberInfo == null)
            return "";
        return memberInfo.GetXmlDocsSummary();
    }
}
