﻿namespace Native.Tool.IniConfig.Exception
{
    /// <summary>
    /// 表示 IniConfig 运行期间发生的错误
    /// </summary>
    public class IniConfigException : System.Exception
    {
        /// <summary>
        /// 初始化 <see cref="IniConfigException"/> 类的新实例
        /// </summary>
        public IniConfigException()
        {
        }

        /// <summary>
        /// 用指定的错误消息初始化 <see cref="IniConfigException"/> 类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public IniConfigException(string message) : base(message)
        {
        }

        /// <summary>
        /// 使用指定的错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="IniConfigException"/> 类的新实例
        /// </summary>
        /// <param name="message">解释异常原因的错误消息</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用（在 Visual Basic 中为 Nothing）</param>
        public IniConfigException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}