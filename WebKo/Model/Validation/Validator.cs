using System;
using System.Collections.Generic;
using System.Text;
using WebKo.Model.General;

namespace WebKo.Model.Validation
{
    public class Validator<T> where T : IEntity
    {
        public T Entity { get; set; }

        public Validator(T entity)
        {
            Entity = entity;
        }

        public bool IsValid()
        {
            return Entity.CheckIfItIsValid();
        }
    }
}
