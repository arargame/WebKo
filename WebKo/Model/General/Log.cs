using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace WebKo.Model.General
{
    public interface ILog : IEntity
    {
        Guid? EntityId { get; set; }
        Entity Entity { get; set; }
        LogType LogType { get; set; }
        string Category { get; set; }
    }

    public enum LogType
    {
        Error,
        Info,
        Warning
    }

    public class Log : Entity , ILog
    {
        public Guid? EntityId { get; set; }

        [NotMapped]
        public virtual Entity Entity { get; set; }

        public string Category { get; set; }

        public LogType LogType { get; set; }

        /// <summary>
        /// Log constructor
        /// </summary>
        /// <param name="category">Kategori(Örnek : sınıf adı)</param>
        /// <param name="name">Ad(Örnek : fonksiyon adı)</param>
        /// <param name="description">Hata ile ilgili açıklama veya durum bilgilendirilmesi(Örnek : ex.ToString();)</param>
        /// <param name="logType">Log kaydının tipi(Örnek : Hata kaydı ise LogType.Error,bilgilendirme ise LogType.Info)</param>
        /// <param name="objectId">Log kaydının sahibi nesne</param>
        public Log(string category, string name, string description, LogType logType = LogType.Error, Guid? objectId = null)
        {
            Category = category;
            Name = name;
            Description = description;
            LogType = logType;
           // BaseObject = BaseObject. ?? null;
        }


        public Log(string description, LogType logType = LogType.Error, string objectId = null)
        {
            //Hatanın hangi sınıf ve fonksiyondan geldiği yakalanıyor
            var methodBase = new StackTrace().GetFrame(1).GetMethod();

            Category = methodBase.DeclaringType.Name;
            Name = methodBase.Name;
            Description = description;
            LogType = logType;
            //ObjectId = objectId;
        }

        public Log() { }

        public static void Create(string description, LogType logType = LogType.Error, Guid? objectId = null)
        {
            var methodBase = new StackTrace().GetFrame(1).GetMethod();

            var category = methodBase.DeclaringType.Name;
            var name = methodBase.Name;

            Log log = new Log(category, name, description, logType, objectId);

            Create(log);
        }
        public static void Create(Log log)
        {
            //new Task(() =>
            //{
            //    using (var context = new Reporting.Core.Data.Context.ReportingContext())
            //    {
            //        var addedLog = context.Log.Add(log);
            //        var isCreated = context.SaveChanges() == 1;
            //    }
            //}).Start();
        }
    }
}
