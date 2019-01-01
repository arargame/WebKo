using System;
using System.Collections.Generic;
using System.Text;
using WebKo.Model.General;
using WebKo.Model.Tracing;

namespace WebKo.Model.Validation
{
    public class Validator<T> where T : Entity
    {
        #region Properties

        public T Entity { get; set; }

        #endregion

        #region Constructor

        public Validator(T entity)
        {
            Entity = entity;
        }

        #endregion

        #region Functions

        /// <summary>
        /// İlgili nesnenin geçerli olup olmadığını sorgular
        /// </summary>
        /// <param name="willBeSaved">Hata olursa loglar kaydedilecek mi?</param>
        /// <returns></returns>
        public bool IsValid(bool willBeSaved, Func<Log, bool> predicate = null)
        {
            Entity.CheckIfItIsValid();

            //if (willBeSaved && Entity.IsValid)
            //    EntityTracer.Save(Entity, predicate);

            return Entity.IsValid;
        }

        #endregion
    }
}
