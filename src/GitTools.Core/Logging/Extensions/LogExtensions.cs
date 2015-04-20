﻿namespace GitTools
{
    using System;
    using System.Runtime.InteropServices;
    using Logging;

    internal static class LogExtensions
    {
        public static void Debug(this ILog log, string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);

            log.Debug(message);
        }

        public static void Info(this ILog log, string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);

            log.Info(message);
        }

        public static void Warning(this ILog log, string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);

            log.Warning(message);
        }

        public static void Error(this ILog log, string messageFormat, params object[] args)
        {
            var message = FormatMessage(messageFormat, args);

            log.Error(message);
        }

        private static string FormatMessage(string messageFormat, params object[] args)
        {
            var message = messageFormat ?? string.Empty;
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            return message;
        }

        /// <summary>
        /// Writes the specified message as error message and then throws the specified exception.
        /// <para/>
        /// The specified exception must have a constructor that accepts a single string as message.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="log">The log.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        /// <example>
        ///   <code>
        /// This example logs an error and immediately throws the exception:<para/>
        ///   <![CDATA[
        /// Log.ErrorAndThrowException<NotSupportedException>("This action is not supported");
        /// ]]>
        ///   </code>
        ///   </example>
        /// <exception cref="ArgumentNullException">The <paramref name="log"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The <typeparamref name="TException"/> does not have a constructor accepting a string.</exception>
        public static void ErrorAndThrowException<TException>(this ILog log, string messageFormat, params object[] args)
            where TException : Exception
        {
            if (log == null)
            {
                return;
            }

            var message = messageFormat ?? string.Empty;
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            log.Error(message);

            Exception exception;

            try
            {
                exception = (Exception)Activator.CreateInstance(typeof(TException), message);
            }
#if !NETFX_CORE && !PCL
            catch (MissingMethodException)
#else
            catch (Exception)
#endif
            {
                var error = string.Format("Exception type '{0}' does not have a constructor accepting a string", typeof(TException).Name);
                log.Error(error);
                throw new NotSupportedException(error);
            }

            throw exception;
        }
    }
}