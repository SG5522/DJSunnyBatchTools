namespace Infrastructure.Utils
{
    public static class StringUtil
    {
        /// <summary>
        /// 原陽信系統取得Oldidentifycard的SN方法
        /// </summary>
        /// <returns></returns>
        public static string GetRandomSN()
            => DateTime.Now.ToString("yyyyMMddfffffff").ToString();        
    }
}
