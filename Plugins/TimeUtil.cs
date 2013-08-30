using System;
using System.Collections;
using System.Collections.Generic;

public class TimeUtil{
	static UInt32 server_time_UTK = 0;
	public static Int32 server_time_zone = 0;
	static DateTime server_time;
	static UInt32 _now;
	static UInt32 _thisDay;
	static UInt32 _nextDay;
	static DateTime _datetime;
	//static bool init_flag = false;
	public static void setServerTime(UInt32 t,Int32 tz)
	{
		//if (init_flag) return;
		//else init_flag = true;
		server_time_UTK = t;
		server_time_zone = tz;
		server_time = DateTime.Now;
		_nextDay = SharpDay(0,server_time_UTK);
		Update();
	}
	public static UInt32 Now{
		get {
			return _now;
		}
	}
	public static UInt32 ThisDay{
		get {
			return _thisDay;
		}
	}
	public static UInt32 NextDay{
		get {
			return _nextDay;
		}
	}
	public static DayOfWeek WeekDay{
		get {
			return _datetime.DayOfWeek;
		}
	}
	public static DateTime NowTime{
		get {
			return _datetime;
		}
	}
	public static void Update()
	{
		TimeSpan ts = DateTime.Now-server_time;
		_now = (UInt32) (server_time_UTK+ts.TotalSeconds);
		_datetime = DateTime.Parse("1970-01-01").AddSeconds(_now-server_time_zone);
		if(_now < _nextDay)
			return;
		UInt32 days = (_now - _nextDay) / 86400;
		_thisDay = _nextDay + days * 86400;
		_nextDay = _thisDay + 86400;
	}
	public static UInt32 SharpDay()
	{
		return SharpDay(0);
	}
	public static UInt32 SharpDay(int c)
	{
		return (UInt32) (_thisDay + c * 86400); 
	}
	public static UInt32 SharpDay(int c,UInt32 tm)
	{
		UInt32 tmptm = (UInt32) ((tm + server_time_zone) / 86400 * 86400 + server_time_zone);
		if(tmptm > tm)
			tmptm -= 86400;
		else if(tmptm + 86400 <= tm)
			tmptm += 86400;
		return (UInt32) (tmptm + c * 86400);
	}
	public static UInt32 SharpHour()
	{
		return SharpHour(0);
	}
	public static UInt32 SharpHour(int c) 
	{ 
		return (UInt32) ((Now + c * 3600) / 3600 * 3600);
	}
	public static UInt32 SharpFourtyMin()
	{
		return SharpFourtyMin(0);
	}
	public static UInt32 SharpFourtyMin(int c) 
	{ 
		return (UInt32) ((Now + c * 2400) / 2400 * 2400);
	}
	public static UInt32 SharpMinute()
	{
		return SharpMinute(0);
	}	
	public static UInt32 SharpMinute(int c) 
	{
		return (UInt32) ((Now + c * 60) / 60 * 60);
	}
	
	public static DateTime LocalTime(UInt32 tm)
	{
		TimeSpan offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);  
		double cur_tm = tm-offset.TotalSeconds-server_time_zone;
		if(cur_tm<0)
			cur_tm+=86400;
		return DateTime.MinValue.AddSeconds(cur_tm);
	}
	public static DateTime ServerTime(UInt32 tm)
	{
		return DateTime.Parse("1970-01-01").AddSeconds(tm-server_time_zone);
	}

	public static uint NextOClock (int c)
	{
		uint todayc = (uint)(ThisDay+3600*c);
		if (Now > todayc) return todayc+86400;
		else return todayc;
	}
}