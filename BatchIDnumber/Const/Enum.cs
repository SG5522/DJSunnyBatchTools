using System.ComponentModel;

namespace BatchIDnumber.Const
{
    public enum IDNumberChangeResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        /// <summary>
        /// 失敗
        /// </summary>
        [Description("失敗")]
        Failure = 1
    }
}
