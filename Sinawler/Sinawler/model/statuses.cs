using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Sinawler.Model
{
	/// <summary>
    /// 类Status。此类中的方法，都是与数据库交互，从中读取或向其中写入数据
	/// </summary>
    /// 
    /*
     <?xml version="1.0" encoding="UTF-8"?>
        <status>
            <created_at>Fri Sep 03 11:41:08 +0800 2010</created_at>
            <id>2265657555</id>
            <text>今日下午回初中看看，然后老同学请吃饭，因为距明天就去报到了，最后的晚餐。希望老天留翻D眼泪等到我军训时先留。。</text>
            <source>
                <a href="http://t.sina.com.cn">新浪微博</a>
            </source>
            <favorited>false</favorited>
            <truncated>false</truncated>
            <geo/>
            <in_reply_to_status_id/>
            <in_reply_to_user_id/>
            <in_reply_to_screen_name/>
            <user>
                <id>1763124584</id>
                <screen_name>标-心</screen_name>
                <name>标-心</name>
                <province>44</province>
                <city>1</city>
                <location>广东 广州</location>
                <description>不帅也不C</description>
                <url>http://blog.163.com/chen_mou/</url>
                <profile_image_url>http://tp1.sinaimg.cn/1763124584/50/1283605697</profile_image_url>
                <domain/>
                <gender>m</gender>
                <followers_count>17</followers_count>
                <friends_count>16</friends_count>
                <statuses_count>71</statuses_count>
                <favourites_count>0</favourites_count>
                <created_at>Thu Jun 17 00:00:00 +0800 2010</created_at>
                <following>false</following>
                <verified>false</verified>
                <allow_all_act_msg>false</allow_all_act_msg>
                <geo_enabled>false</geo_enabled>
            </user>
        </status>
    */

    public class Status
	{
        static private Database db = DatabaseFactory.CreateDatabase();

        public Status()
		{}

		#region Model
		private long _status_id;
		private string _created_at;
		private string _content;
		private string _source_url;
		private string _source_name;
		private bool _favorited;
		private bool _truncated;
        private string _geo;
		private long _in_reply_to_status_id;
		private long _in_reply_to_user_id;
		private string _in_reply_to_screen_name;
		private string _thumbnail_pic="";
		private string _bmiddle_pic="";
		private string _original_pic="";
		private long _uid;
		private long _retweeted_status_id=0;
        private int _iteration;
        private string _update_time;
		/// <summary>
		/// 微博ID（XML中为id）
		/// </summary>
		public long status_id
		{
			set{ _status_id=value;}
			get{return _status_id;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public string created_at
		{
			set{ _created_at=value;}
			get{return _created_at;}
		}
		/// <summary>
		/// 微博信息内容(XML中为text)
		/// </summary>
		public string content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 微博来源中的URL
		/// </summary>
		public string source_url
		{
			set{ _source_url=value;}
			get{return _source_url;}
		}
		/// <summary>
		/// 微博来源的名字
		/// </summary>
		public string source_name
		{
			set{ _source_name=value;}
			get{return _source_name;}
		}
		/// <summary>
		/// 是否已收藏（应该是对于当前登录账号）
		/// </summary>
		public bool favorited
		{
			set{ _favorited=value;}
			get{return _favorited;}
		}
		/// <summary>
		/// 是否被截断
		/// </summary>
		public bool truncated
		{
			set{ _truncated=value;}
			get{return _truncated;}
		}
        /// <summary>
        /// 地理信息（暂不明类型）
        /// </summary>
        public string geo
        {
            set { _geo = value; }
            get { return _geo; }
        }
		/// <summary>
		/// 回复ID
		/// </summary>
		public long in_reply_to_status_id
		{
			set{ _in_reply_to_status_id=value;}
			get{return _in_reply_to_status_id;}
		}
		/// <summary>
		/// 回复人UID
		/// </summary>
		public long in_reply_to_user_id
		{
			set{ _in_reply_to_user_id=value;}
			get{return _in_reply_to_user_id;}
		}
		/// <summary>
		/// 回复人昵称
		/// </summary>
		public string in_reply_to_screen_name
		{
			set{ _in_reply_to_screen_name=value;}
			get{return _in_reply_to_screen_name;}
		}
		/// <summary>
		/// 缩略图
		/// </summary>
		public string thumbnail_pic
		{
			set{ _thumbnail_pic=value;}
			get{return _thumbnail_pic;}
		}
		/// <summary>
		/// 中型图片
		/// </summary>
		public string bmiddle_pic
		{
			set{ _bmiddle_pic=value;}
			get{return _bmiddle_pic;}
		}
		/// <summary>
		/// 原始图片
		/// </summary>
		public string original_pic
		{
			set{ _original_pic=value;}
			get{return _original_pic;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public long uid
		{
			set{ _uid=value;}
			get{return _uid;}
		}
		/// <summary>
		/// 同时转发的微博ID
		/// </summary>
		public long retweeted_status_id
		{
			set{ _retweeted_status_id=value;}
			get{return _retweeted_status_id;}
		}
        /// <summary>
        /// 迭代次数。默认为0，每迭代一次，就加1，则为0的为最近的数据
        /// </summary>
        public int iteration
        {
            set { _iteration = value; }
            get { return _iteration; }
        }
        /// <summary>
        /// 记录更新时间
        /// </summary>
        public string update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
		#endregion Model

		#region  成员方法

		static public bool Exists(long lStatusID)
		{
            int count = db.CountByExecuteSQLSelect( "select status_id from statuses where status_id=" + lStatusID.ToString() );
            return count > 0;
		}

        /// <summary>
        /// 更新数据库中已有数据的迭代次数
        /// </summary>
        static public void NewIterate ()
        {
            db.CountByExecuteSQL( "update statuses set iteration=iteration+1" );
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add()
		{
            try
            {
                Hashtable htValues = new Hashtable();
                _update_time = "'" + DateTime.Now.ToString( "u" ).Replace( "Z", "" ) + "'";
                htValues.Add( "status_id", _status_id );
                htValues.Add( "created_at", "'" + _created_at + "'" );
                htValues.Add( "content", "'" + _content.Replace( "'", "''" ) + "'" );
                htValues.Add( "source_url", "'" + _source_url + "'" );
                htValues.Add( "source_name", "'" + _source_name + "'" );
                if (_favorited)
                    htValues.Add( "favorited", 1 );
                else
                    htValues.Add( "favorited", 0 );
                if (_truncated)
                    htValues.Add( "truncated", 1 );
                else
                    htValues.Add( "truncated", 0 );
                htValues.Add( "geo", "'" + _geo.Replace( "'", "''" ) + "'" );
                htValues.Add( "in_reply_to_status_id", _in_reply_to_status_id );
                htValues.Add( "in_reply_to_user_id", _in_reply_to_user_id );
                htValues.Add( "in_reply_to_screen_name", "'" + _in_reply_to_screen_name.Replace( "'", "''" ) + "'" );
                htValues.Add( "thumbnail_pic", "'" + _thumbnail_pic.Replace( "'", "''" ) + "'" );
                htValues.Add( "bmiddle_pic", "'" + _bmiddle_pic.Replace( "'", "''" ) + "'" );
                htValues.Add( "original_pic", "'" + _original_pic.Replace( "'", "''" ) + "'" );
                htValues.Add( "uid", _uid );
                htValues.Add( "retweeted_status_id", _retweeted_status_id );
                htValues.Add( "iteration", iteration );
                htValues.Add( "update_time", _update_time );

                db.Insert( "statuses", htValues );
            }
            catch
            { return; }
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public void GetModel(long status_id)
		{
            //StringBuilder strSql=new StringBuilder();
            //strSql.Append("select  top 1 status_id,created_at,content,source_url,source_name,favorited,truncated,in_reply_to_status_id,in_reply_to_user_id,in_reply_to_screen_name,thumbnail_pic,bmiddle_pic,original_pic,uid,retweeted_status_id ");
            //strSql.Append(" FROM statuses ");
            //strSql.Append(" where status_id=@status_id ");
            //SqlParameter[] parameters = {
            //        new SqlParameter("@status_id", SqlDbType.BigInt)};
            //parameters[0].Value = status_id;

            //DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
            //if(ds.Tables[0].Rows.Count>0)
            //{
            //    if(ds.Tables[0].Rows[0]["status_id"].ToString()!="")
            //    {
            //        model.status_id=long.Parse(ds.Tables[0].Rows[0]["status_id"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["created_at"].ToString()!="")
            //    {
            //        model.created_at=DateTime.Parse(ds.Tables[0].Rows[0]["created_at"].ToString());
            //    }
            //    model.content=ds.Tables[0].Rows[0]["content"].ToString();
            //    model.source_url=ds.Tables[0].Rows[0]["source_url"].ToString();
            //    model.source_name=ds.Tables[0].Rows[0]["source_name"].ToString();
            //    if(ds.Tables[0].Rows[0]["favorited"].ToString()!="")
            //    {
            //        if((ds.Tables[0].Rows[0]["favorited"].ToString()=="1")||(ds.Tables[0].Rows[0]["favorited"].ToString().ToLower()=="true"))
            //        {
            //            model.favorited=true;
            //        }
            //        else
            //        {
            //            model.favorited=false;
            //        }
            //    }
            //    if(ds.Tables[0].Rows[0]["truncated"].ToString()!="")
            //    {
            //        if((ds.Tables[0].Rows[0]["truncated"].ToString()=="1")||(ds.Tables[0].Rows[0]["truncated"].ToString().ToLower()=="true"))
            //        {
            //            model.truncated=true;
            //        }
            //        else
            //        {
            //            model.truncated=false;
            //        }
            //    }
            //    if(ds.Tables[0].Rows[0]["in_reply_to_status_id"].ToString()!="")
            //    {
            //        model.in_reply_to_status_id=long.Parse(ds.Tables[0].Rows[0]["in_reply_to_status_id"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["in_reply_to_user_id"].ToString()!="")
            //    {
            //        model.in_reply_to_user_id=long.Parse(ds.Tables[0].Rows[0]["in_reply_to_user_id"].ToString());
            //    }
            //    model.in_reply_to_screen_name=ds.Tables[0].Rows[0]["in_reply_to_screen_name"].ToString();
            //    model.thumbnail_pic=ds.Tables[0].Rows[0]["thumbnail_pic"].ToString();
            //    model.bmiddle_pic=ds.Tables[0].Rows[0]["bmiddle_pic"].ToString();
            //    model.original_pic=ds.Tables[0].Rows[0]["original_pic"].ToString();
            //    if(ds.Tables[0].Rows[0]["uid"].ToString()!="")
            //    {
            //        model.uid=long.Parse(ds.Tables[0].Rows[0]["uid"].ToString());
            //    }
            //    if(ds.Tables[0].Rows[0]["retweeted_status_id"].ToString()!="")
            //    {
            //        model.retweeted_status_id=long.Parse(ds.Tables[0].Rows[0]["retweeted_status_id"].ToString());
            //    }
            //}
		}

        /// <summary>
        /// 获得数据库中指定用户最新一条微博ID
        /// </summary>
        static public long GetLastStatusIDOf(long lUid)
        {
            string strSQL = "select top 1 status_id from statuses where uid="+lUid.ToString()+" order by created_at desc";
            DataRow dr = db.GetDataRow( strSQL );
            if (dr == null) return 0;
            else return Convert.ToInt64( dr[0] );
        }

		#endregion  成员方法
	}
}

