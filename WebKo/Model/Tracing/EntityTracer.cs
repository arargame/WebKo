using System;
using System.Collections.Generic;
using System.Text;
using WebKo.Model.General;

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

        #endregion
    }
}
