namespace Infrastructure.Utils
{
    public static class StringUtil
    {
        public static string GetRandomSn()        
            => DateTime.Now.ToString("yyyyMMddffff") + new Random().Next(11, 99).ToString();        
    }
}
