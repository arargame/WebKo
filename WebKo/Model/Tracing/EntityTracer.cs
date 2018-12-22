using System;
using System.Collections.Generic;
using System.Text;
using WebKo.Model.General;
using System.Linq;

namespace WebKo.Model.Tracing
{
    //Hafızada tutulan nesnelerin süreç bilgisi,validasyonda kullanılır
    public class EntityTracer
    {
        #region Properties

        public Entity Entity { get; set; }

        public string Key { get; set; }

        public string Message { get; set; }

        #endregion

        #region Constructor

        public EntityTracer() { }

        public EntityTracer(string key,string message)
        {
            Key = key;
            Message = message;
        }

        #endregion

        #region Functions

        public Log ToLog(LogType logType = LogType.Error)
        {
            return new Log(Entity.GetType().Name, Key, Message, logType, Entity.Id);
        }

        //dil desteği lazım
        //public string Display
        //{
        //    get
        //    {
        //        //Key : Class Message : Error
        //        return string.Format("{}",);
        //    }
        //}

        /// <summary>
        /// Gerektiğinde bir entity'nin üzerindeki bütün tracer'ları kaydetmeye yarar
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="predicate">LogType'ı Error olanlar gibi filtreler yapılabilir</param>
        public static void Save(Entity entity,Func<Log,bool> predicate = null)
        {
            var logs = entity.EntityTracers
                                .Select(et => et.ToLog());

            if (predicate != null)
                logs = logs.Where(predicate);

            foreach (var log in logs)
            {
                Log.Create(log);
            }
        }

        #endregion
    }
}
