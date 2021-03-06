﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/10/30 19:49:40 (UTC +8:00)
** desc： ...
*************************************************/
using AutumnBox.GUI.Util.Debugging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutumnBox.GUI.Util.Net
{
    internal abstract class JsonGetter<TResultObject>
    {
        protected readonly WebClient webClient;

        public abstract string Url { get; }

        public JsonGetter()
        {
            webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
        }

        public void Try(Action<TResultObject> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            Task.Run(() =>
            {
                try
                {
                    callback(GetSync());
                }
                catch (Exception e)
                {
                    SLogger.Warn(this, $"The operation of get and parse json from {Url} is failed", e);
                }
            });
        }

        public TResultObject GetSync()
        {
            return ParseJson(GetJson());
        }

        protected virtual TResultObject ParseJson(string json)
        {
            var obj = JsonConvert.DeserializeObject<TResultObject>(json);
            return obj;
        }

        protected virtual string GetJson()
        {
            return webClient.DownloadString(Url);
        }
    }
}
