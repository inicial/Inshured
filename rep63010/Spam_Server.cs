using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using ltp_v2.Framework;

namespace rep6050
{
    public partial class LTS_SpamServerAttach : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _LTSA_ID;

        private int _LTSA_LSSId;

        private System.Data.Linq.Binary _LTSA_Source;

        private string _LTSA_Extension;

        private EntityRef<LTS_SpamServer> _LTS_SpamServer;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnLTSA_IDChanging(int value);
        partial void OnLTSA_IDChanged();
        partial void OnLTSA_LSSIdChanging(int value);
        partial void OnLTSA_LSSIdChanged();
        partial void OnLTSA_SourceChanging(System.Data.Linq.Binary value);
        partial void OnLTSA_SourceChanged();
        partial void OnLTSA_ExtensionChanging(string value);
        partial void OnLTSA_ExtensionChanged();
        #endregion

        public LTS_SpamServerAttach()
        {
            this._LTS_SpamServer = default(EntityRef<LTS_SpamServer>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LTSA_ID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int LTSA_ID
        {
            get
            {
                return this._LTSA_ID;
            }
            set
            {
                if ((this._LTSA_ID != value))
                {
                    this.OnLTSA_IDChanging(value);
                    this.SendPropertyChanging();
                    this._LTSA_ID = value;
                    this.SendPropertyChanged("LTSA_ID");
                    this.OnLTSA_IDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LTSA_LSSId", DbType = "Int NOT NULL")]
        public int LTSA_LSSId
        {
            get
            {
                return this._LTSA_LSSId;
            }
            set
            {
                if ((this._LTSA_LSSId != value))
                {
                    if (this._LTS_SpamServer.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnLTSA_LSSIdChanging(value);
                    this.SendPropertyChanging();
                    this._LTSA_LSSId = value;
                    this.SendPropertyChanged("LTSA_LSSId");
                    this.OnLTSA_LSSIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LTSA_Source", DbType = "VarBinary(MAX) NOT NULL", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public System.Data.Linq.Binary LTSA_Source
        {
            get
            {
                return this._LTSA_Source;
            }
            set
            {
                if ((this._LTSA_Source != value))
                {
                    this.OnLTSA_SourceChanging(value);
                    this.SendPropertyChanging();
                    this._LTSA_Source = value;
                    this.SendPropertyChanged("LTSA_Source");
                    this.OnLTSA_SourceChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LTSA_Extension", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string LTSA_Extension
        {
            get
            {
                return this._LTSA_Extension;
            }
            set
            {
                if ((this._LTSA_Extension != value))
                {
                    this.OnLTSA_ExtensionChanging(value);
                    this.SendPropertyChanging();
                    this._LTSA_Extension = value;
                    this.SendPropertyChanged("LTSA_Extension");
                    this.OnLTSA_ExtensionChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "LTS_SpamServer_LTS_SpamServerAttach", Storage = "_LTS_SpamServer", ThisKey = "LTSA_LSSId", OtherKey = "LSS_ID", IsForeignKey = true)]
        public LTS_SpamServer LTS_SpamServer
        {
            get
            {
                return this._LTS_SpamServer.Entity;
            }
            set
            {
                LTS_SpamServer previousValue = this._LTS_SpamServer.Entity;
                if (((previousValue != value)
                            || (this._LTS_SpamServer.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._LTS_SpamServer.Entity = null;
                        previousValue.LTS_SpamServerAttaches.Remove(this);
                    }
                    this._LTS_SpamServer.Entity = value;
                    if ((value != null))
                    {
                        value.LTS_SpamServerAttaches.Add(this);
                        this._LTSA_LSSId = value.LSS_ID;
                    }
                    else
                    {
                        this._LTSA_LSSId = default(int);
                    }
                    this.SendPropertyChanged("LTS_SpamServer");
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
	
    
    
    
    public partial class LTS_SpamServer : INotifyPropertyChanging, INotifyPropertyChanged
    {
        partial void OnCreated()
        {
            this._LSS_DTEndPeriod = DateTime.Now;
            this._LSS_ServiceSend = "Inshured";

            //if (SqlConnection.ConnectionUserInformation.AccessForAgency != null
            //    && !String.IsNullOrEmpty(SqlConnection.ConnectionUserInformation.AccessForAgency.LTP_AC_BackMail))
            //{
            //    this._LSS_MailFrom = SqlConnection.ConnectionUserInformation.US_NAME + " " +
            //        SqlConnection.ConnectionUserInformation.US_FNAME + "<" +
            //        SqlConnection.ConnectionUserInformation.AccessForAgency.LTP_AC_BackMail + ">";
            //}
            //else
            //{
                this._LSS_MailFrom = "robot@mcruises.ru";
            //}
        }
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _LSS_ID;

        private string _LSS_MailFrom;

        private string _LSS_MailTo;

        private string _LSS_Subject;

        private string _LSS_Body;

        private System.Nullable<System.DateTime> _LSS_DTEndPeriod = default(System.Nullable<System.DateTime>);

        private string _LSS_ServiceSend;

        private int _LSS_PRKey;

        private System.Nullable<System.DateTime> _LSS_DTSend = default(System.Nullable<System.DateTime>);

        private EntitySet<LTS_SpamServerAttach> _LTS_SpamServerAttaches;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnLSS_IDChanging(int value);
        partial void OnLSS_IDChanged();
        partial void OnLSS_MailFromChanging(string value);
        partial void OnLSS_MailFromChanged();
        partial void OnLSS_MailToChanging(string value);
        partial void OnLSS_MailToChanged();
        partial void OnLSS_SubjectChanging(string value);
        partial void OnLSS_SubjectChanged();
        partial void OnLSS_BodyChanging(string value);
        partial void OnLSS_BodyChanged();
        partial void OnLSS_ServiceSendChanging(string value);
        partial void OnLSS_ServiceSendChanged();
        partial void OnLSS_PRKeyChanging(int value);
        partial void OnLSS_PRKeyChanged();
        #endregion

        public LTS_SpamServer()
        {
            this._LTS_SpamServerAttaches = new EntitySet<LTS_SpamServerAttach>(new Action<LTS_SpamServerAttach>(this.attach_LTS_SpamServerAttaches), new Action<LTS_SpamServerAttach>(this.detach_LTS_SpamServerAttaches));
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_ID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int LSS_ID
        {
            get
            {
                return this._LSS_ID;
            }
            set
            {
                if ((this._LSS_ID != value))
                {
                    this.OnLSS_IDChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_ID = value;
                    this.SendPropertyChanged("LSS_ID");
                    this.OnLSS_IDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_MailFrom", DbType = "VarChar(150)")]
        public string LSS_MailFrom
        {
            get
            {
                return this._LSS_MailFrom;
            }
            set
            {
                if ((this._LSS_MailFrom != value))
                {
                    this.OnLSS_MailFromChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_MailFrom = value;
                    this.SendPropertyChanged("LSS_MailFrom");
                    this.OnLSS_MailFromChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_MailTo", DbType = "VarChar(150)")]
        public string LSS_MailTo
        {
            get
            {
                return this._LSS_MailTo;
            }
            set
            {
                if ((this._LSS_MailTo != value))
                {
                    this.OnLSS_MailToChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_MailTo = value;
                    this.SendPropertyChanged("LSS_MailTo");
                    this.OnLSS_MailToChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_Subject", DbType = "VarChar(255) NOT NULL", CanBeNull = false)]
        public string LSS_Subject
        {
            get
            {
                return this._LSS_Subject;
            }
            set
            {
                if ((this._LSS_Subject != value))
                {
                    this.OnLSS_SubjectChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_Subject = value;
                    this.SendPropertyChanged("LSS_Subject");
                    this.OnLSS_SubjectChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_Body", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string LSS_Body
        {
            get
            {
                return this._LSS_Body;
            }
            set
            {
                if ((this._LSS_Body != value))
                {
                    this.OnLSS_BodyChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_Body = value;
                    this.SendPropertyChanged("LSS_Body");
                    this.OnLSS_BodyChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_DTEndPeriod", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<System.DateTime> LSS_DTEndPeriod
        {
            get
            {
                return this._LSS_DTEndPeriod;
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_ServiceSend", DbType = "VarChar(50)")]
        public string LSS_ServiceSend
        {
            get
            {
                return this._LSS_ServiceSend;
            }
            set
            {
                if ((this._LSS_ServiceSend != value))
                {
                    this.OnLSS_ServiceSendChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_ServiceSend = value;
                    this.SendPropertyChanged("LSS_ServiceSend");
                    this.OnLSS_ServiceSendChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_PRKey", DbType = "Int NOT NULL")]
        public int LSS_PRKey
        {
            get
            {
                return this._LSS_PRKey;
            }
            set
            {
                if ((this._LSS_PRKey != value))
                {
                    this.OnLSS_PRKeyChanging(value);
                    this.SendPropertyChanging();
                    this._LSS_PRKey = value;
                    this.SendPropertyChanged("LSS_PRKey");
                    this.OnLSS_PRKeyChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LSS_DTSend", DbType = "DateTime", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<System.DateTime> LSS_DTSend
        {
            get
            {
                return this._LSS_DTSend;
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "LTS_SpamServer_LTS_SpamServerAttach", Storage = "_LTS_SpamServerAttaches", ThisKey = "LSS_ID", OtherKey = "LTSA_LSSId")]
        public EntitySet<LTS_SpamServerAttach> LTS_SpamServerAttaches
        {
            get
            {
                return this._LTS_SpamServerAttaches;
            }
            set
            {
                this._LTS_SpamServerAttaches.Assign(value);
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void attach_LTS_SpamServerAttaches(LTS_SpamServerAttach entity)
        {
            this.SendPropertyChanging();
            entity.LTS_SpamServer = this;
        }

        private void detach_LTS_SpamServerAttaches(LTS_SpamServerAttach entity)
        {
            this.SendPropertyChanging();
            entity.LTS_SpamServer = null;
        }
    }
}
