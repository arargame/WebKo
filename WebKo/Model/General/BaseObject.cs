using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebKo.Model.General
{
    public interface IHasCode
    {
        string Code { get; set; }
    }

    public interface IBaseObject
    {
        Guid Id { get; set; }

        //string LogId { get; set; }

        string Name { get; set; }

        string FullName { get; set; }

        string DisplayName { get; }

        string Description { get; set; }

        bool IsValid { get; set; }

        //string RecorderInfo { get; set; }

        //DateTime? RecordDate { get; set; }

        //string UpdaterInfo { get; set; }

        //DateTime? UpdateDate { get; set; }

        ICollection<Log> Logs { get; set; }

        //string Platform { get; set; }

        //List<ModelError> ModelErrors { get; set; }

        //void AddModelError(params ModelError[] modelError);

        bool CheckIfItIsValid();

        //Employee CurrentEmployee { get; set; }

        //void SetCurrentEmployee(string personnelCode);

        ////void SetCurrentEmployee(Employee employee);

        //IBaseObject SetLogId(string logIdbool);

        //List<Log> GetAllLogs();

        //void AddLog(Log log, bool enableToSave = false);

        //IBaseObject SetTableName(string tableName = null);

        //IBaseObject Load();
    }

    public abstract class BaseObject : IBaseObject
    {
        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength]
        public string Description { get; set; }

        public bool IsValid { get; set; }

        public virtual string DisplayName
        {
            get
            {
                return Name;
            }
        }
        #endregion

        #region Collections
        public virtual ICollection<Log> Logs { get; set; }
        #endregion

        #region Constructor
        public BaseObject()
        {
            Initialize();
        }
        #endregion

        #region Functions
        public virtual BaseObject Initialize()
        {
            IsValid = true;

            Id = Guid.NewGuid();

            return this;
        }

        public virtual bool CheckIfItIsValid()
        {
            return IsValid;
        }
        #endregion
    }
}
