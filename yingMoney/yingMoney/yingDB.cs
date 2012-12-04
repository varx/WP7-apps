﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5448
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



[System.Data.Linq.Mapping.DatabaseAttribute(Name="yingDB")]
public partial class YingDB : System.Data.Linq.DataContext
{
	
	private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
	
  #region Extensibility Method Definitions
  partial void OnCreated();
  partial void InsertHome_info(Home_info instance);
  partial void UpdateHome_info(Home_info instance);
  partial void DeleteHome_info(Home_info instance);
  partial void InsertMain_type(Main_type instance);
  partial void UpdateMain_type(Main_type instance);
  partial void DeleteMain_type(Main_type instance);
  partial void InsertOutgoing(Outgoing instance);
  partial void UpdateOutgoing(Outgoing instance);
  partial void DeleteOutgoing(Outgoing instance);
  partial void InsertSub_type(Sub_type instance);
  partial void UpdateSub_type(Sub_type instance);
  partial void DeleteSub_type(Sub_type instance);
  #endregion
	
	public YingDB(string connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
    //public YingDB(System.Data.IDbConnection connection) : 
    //        base(connection, mappingSource)
    //{
    //    OnCreated();
    //}
	
	public YingDB(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
    //public YingDB(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
    //        base(connection, mappingSource)
    //{
    //    OnCreated();
    //}
	
	public System.Data.Linq.Table<Home_info> Home_info
	{
		get
		{
			return this.GetTable<Home_info>();
		}
	}
	
	public System.Data.Linq.Table<Main_type> Main_type
	{
		get
		{
			return this.GetTable<Main_type>();
		}
	}
	
	public System.Data.Linq.Table<Outgoing> Outgoing
	{
		get
		{
			return this.GetTable<Outgoing>();
		}
	}
	
	public System.Data.Linq.Table<Sub_type> Sub_type
	{
		get
		{
			return this.GetTable<Sub_type>();
		}
	}
}

[Table(Name="home_info")]
public partial class Home_info : INotifyPropertyChanging, INotifyPropertyChanged
{
	
	private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
	
	private long _Daysum;
	
	private long _Weeksum;
	
	private long _Mouthsum;
	
	private System.DateTime _Date;
	
	private System.DateTime _Weekstart;
	
	private int _Id;
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDaysumChanging(long value);
    partial void OnDaysumChanged();
    partial void OnWeeksumChanging(long value);
    partial void OnWeeksumChanged();
    partial void OnMouthsumChanging(long value);
    partial void OnMouthsumChanged();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnWeekstartChanging(System.DateTime value);
    partial void OnWeekstartChanged();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    #endregion
	
	public Home_info()
	{
		OnCreated();
	}
	
	[Column(Name="daysum", Storage="_Daysum", DbType="BigInt NOT NULL")]
	public long Daysum
	{
		get
		{
			return this._Daysum;
		}
		set
		{
			if ((this._Daysum != value))
			{
				this.OnDaysumChanging(value);
				this.SendPropertyChanging();
				this._Daysum = value;
				this.SendPropertyChanged("Daysum");
				this.OnDaysumChanged();
			}
		}
	}
	
	[Column(Name="weeksum", Storage="_Weeksum", DbType="BigInt NOT NULL")]
	public long Weeksum
	{
		get
		{
			return this._Weeksum;
		}
		set
		{
			if ((this._Weeksum != value))
			{
				this.OnWeeksumChanging(value);
				this.SendPropertyChanging();
				this._Weeksum = value;
				this.SendPropertyChanged("Weeksum");
				this.OnWeeksumChanged();
			}
		}
	}
	
	[Column(Name="mouthsum", Storage="_Mouthsum", DbType="BigInt NOT NULL")]
	public long Mouthsum
	{
		get
		{
			return this._Mouthsum;
		}
		set
		{
			if ((this._Mouthsum != value))
			{
				this.OnMouthsumChanging(value);
				this.SendPropertyChanging();
				this._Mouthsum = value;
				this.SendPropertyChanged("Mouthsum");
				this.OnMouthsumChanged();
			}
		}
	}
	
	[Column(Name="date", Storage="_Date", DbType="DateTime NOT NULL")]
	public System.DateTime Date
	{
		get
		{
			return this._Date;
		}
		set
		{
			if ((this._Date != value))
			{
				this.OnDateChanging(value);
				this.SendPropertyChanging();
				this._Date = value;
				this.SendPropertyChanged("Date");
				this.OnDateChanged();
			}
		}
	}
	
	[Column(Name="weekstart", Storage="_Weekstart", DbType="DateTime NOT NULL")]
	public System.DateTime Weekstart
	{
		get
		{
			return this._Weekstart;
		}
		set
		{
			if ((this._Weekstart != value))
			{
				this.OnWeekstartChanging(value);
				this.SendPropertyChanging();
				this._Weekstart = value;
				this.SendPropertyChanged("Weekstart");
				this.OnWeekstartChanged();
			}
		}
	}
	
	[Column(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
	public int Id
	{
		get
		{
			return this._Id;
		}
		set
		{
			if ((this._Id != value))
			{
				this.OnIdChanging(value);
				this.SendPropertyChanging();
				this._Id = value;
				this.SendPropertyChanged("Id");
				this.OnIdChanged();
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

[Table(Name="main_type")]
public partial class Main_type : INotifyPropertyChanging, INotifyPropertyChanged
{
	
	private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
	
	private int _Id;
	
	private string _Name;
	
	private byte _Delete;
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDeleteChanging(byte value);
    partial void OnDeleteChanged();
    #endregion
	
	public Main_type()
	{
		OnCreated();
	}
	
	[Column(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
	public int Id
	{
		get
		{
			return this._Id;
		}
		set
		{
			if ((this._Id != value))
			{
				this.OnIdChanging(value);
				this.SendPropertyChanging();
				this._Id = value;
				this.SendPropertyChanged("Id");
				this.OnIdChanged();
			}
		}
	}
	
	[Column(Name="name", Storage="_Name", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
	public string Name
	{
		get
		{
			return this._Name;
		}
		set
		{
			if ((this._Name != value))
			{
				this.OnNameChanging(value);
				this.SendPropertyChanging();
				this._Name = value;
				this.SendPropertyChanged("Name");
				this.OnNameChanged();
			}
		}
	}
	
	[Column(Name="delete", Storage="_Delete", DbType="TinyInt NOT NULL")]
	public byte Delete
	{
		get
		{
			return this._Delete;
		}
		set
		{
			if ((this._Delete != value))
			{
				this.OnDeleteChanging(value);
				this.SendPropertyChanging();
				this._Delete = value;
				this.SendPropertyChanged("Delete");
				this.OnDeleteChanged();
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

[Table(Name="outgoing")]
public partial class Outgoing : INotifyPropertyChanging, INotifyPropertyChanged
{
	
	private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
	
	private int _Id;
	
	private int _Main_id;
	
	private int _Sub_id;
	
	private int _Money;
	
	private string _Comment;
	
	private System.DateTime _Time;
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnMain_idChanging(int value);
    partial void OnMain_idChanged();
    partial void OnSub_idChanging(int value);
    partial void OnSub_idChanged();
    partial void OnMoneyChanging(int value);
    partial void OnMoneyChanged();
    partial void OnCommentChanging(string value);
    partial void OnCommentChanged();
    partial void OnTimeChanging(System.DateTime value);
    partial void OnTimeChanged();
    #endregion
	
	public Outgoing()
	{
		OnCreated();
	}
	
	[Column(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
	public int Id
	{
		get
		{
			return this._Id;
		}
		set
		{
			if ((this._Id != value))
			{
				this.OnIdChanging(value);
				this.SendPropertyChanging();
				this._Id = value;
				this.SendPropertyChanged("Id");
				this.OnIdChanged();
			}
		}
	}
	
	[Column(Name="main_id", Storage="_Main_id", DbType="Int NOT NULL")]
	public int Main_id
	{
		get
		{
			return this._Main_id;
		}
		set
		{
			if ((this._Main_id != value))
			{
				this.OnMain_idChanging(value);
				this.SendPropertyChanging();
				this._Main_id = value;
				this.SendPropertyChanged("Main_id");
				this.OnMain_idChanged();
			}
		}
	}
	
	[Column(Name="sub_id", Storage="_Sub_id", DbType="Int NOT NULL")]
	public int Sub_id
	{
		get
		{
			return this._Sub_id;
		}
		set
		{
			if ((this._Sub_id != value))
			{
				this.OnSub_idChanging(value);
				this.SendPropertyChanging();
				this._Sub_id = value;
				this.SendPropertyChanged("Sub_id");
				this.OnSub_idChanged();
			}
		}
	}
	
	[Column(Name="money", Storage="_Money", DbType="Int NOT NULL")]
	public int Money
	{
		get
		{
			return this._Money;
		}
		set
		{
			if ((this._Money != value))
			{
				this.OnMoneyChanging(value);
				this.SendPropertyChanging();
				this._Money = value;
				this.SendPropertyChanged("Money");
				this.OnMoneyChanged();
			}
		}
	}
	
	[Column(Name="comment", Storage="_Comment", DbType="NVarChar(100)")]
	public string Comment
	{
		get
		{
			return this._Comment;
		}
		set
		{
			if ((this._Comment != value))
			{
				this.OnCommentChanging(value);
				this.SendPropertyChanging();
				this._Comment = value;
				this.SendPropertyChanged("Comment");
				this.OnCommentChanged();
			}
		}
	}
	
	[Column(Name="time", Storage="_Time", DbType="DateTime NOT NULL")]
	public System.DateTime Time
	{
		get
		{
			return this._Time;
		}
		set
		{
			if ((this._Time != value))
			{
				this.OnTimeChanging(value);
				this.SendPropertyChanging();
				this._Time = value;
				this.SendPropertyChanged("Time");
				this.OnTimeChanged();
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

[Table(Name="sub_type")]
public partial class Sub_type : INotifyPropertyChanging, INotifyPropertyChanged
{
	
	private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
	
	private int _Id;
	
	private int _Pid;
	
	private string _Name;
	
	private byte _Delete;
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnPidChanging(int value);
    partial void OnPidChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnDeleteChanging(byte value);
    partial void OnDeleteChanged();
    #endregion
	
	public Sub_type()
	{
		OnCreated();
	}
	
	[Column(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
	public int Id
	{
		get
		{
			return this._Id;
		}
		set
		{
			if ((this._Id != value))
			{
				this.OnIdChanging(value);
				this.SendPropertyChanging();
				this._Id = value;
				this.SendPropertyChanged("Id");
				this.OnIdChanged();
			}
		}
	}
	
	[Column(Name="pid", Storage="_Pid", DbType="Int NOT NULL")]
	public int Pid
	{
		get
		{
			return this._Pid;
		}
		set
		{
			if ((this._Pid != value))
			{
				this.OnPidChanging(value);
				this.SendPropertyChanging();
				this._Pid = value;
				this.SendPropertyChanged("Pid");
				this.OnPidChanged();
			}
		}
	}
	
	[Column(Name="name", Storage="_Name", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
	public string Name
	{
		get
		{
			return this._Name;
		}
		set
		{
			if ((this._Name != value))
			{
				this.OnNameChanging(value);
				this.SendPropertyChanging();
				this._Name = value;
				this.SendPropertyChanged("Name");
				this.OnNameChanged();
			}
		}
	}
	
	[Column(Name="delete", Storage="_Delete", DbType="TinyInt NOT NULL")]
	public byte Delete
	{
		get
		{
			return this._Delete;
		}
		set
		{
			if ((this._Delete != value))
			{
				this.OnDeleteChanging(value);
				this.SendPropertyChanging();
				this._Delete = value;
				this.SendPropertyChanged("Delete");
				this.OnDeleteChanged();
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
#pragma warning restore 1591
