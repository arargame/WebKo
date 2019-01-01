using System;
using System.Collections.Generic;
using System.Text;
using WebKo.Model.General;
using System.Linq;

namespace WebKo.Model.Tracing
{
    public enum TracerValueState
    {
        Created,
        Updated,
        Deleted
    }

    //Hafızada tutulan nesnelerin süreç bilgisi,validasyonda kullanılır
    public class EntityTracer
    {
        #region Properties

        public Entity Entity { get; set; }

        public string Key { get; set; }

        public string Message { get; set; }

        public LogType LogType { get; set; }

        public TracerValueState? ValueState { get; set; }

        public string PreviousValue { get; set; }

        public string NewValue { get; set; }

        #endregion

        #region Constructor

        public EntityTracer() { }

        public EntityTracer(string key, string message, LogType logType =  LogType.Info, TracerValueState? valueState = null)
        {
            Key = key;
            Message = valueState!=null ? valueState.ToString() : message;
            LogType = valueState != null ? Model.General.LogType.Warning : logType;
        }

        #endregion

        #region Functions

        public Log ToLog()
        {
            return new Log(Entity.GetType().Name, Key, Message, LogType, Entity.Id);
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

        public void Save()
        {
            Log.Create(ToLog());
        }

        #endregion
    }
}
