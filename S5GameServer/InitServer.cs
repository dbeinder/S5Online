﻿using S5GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace S5GameServer
{
    public class ServerDefinition
    {
        public string Host;
        public int Port;
    }

    public class InitServer
    {
        static ISimpleLogger logger;
        static string initResponse;
        static WebServer ws;

        public static void Run()
        {
            Run(NoLogger.Instance);
        }

        public static void Run(ISimpleLogger logger)
        {
            InitServer.logger = logger;
            BuildInitResponse();
            ws = new WebServer(new[] { "http://*:" + ServerConfig.Instance.InitPort.ToString() + "/" }, HandleRequest);
            ws.Run();
        }

        static void BuildInitResponse()
        {
            var template = "[Servers]\n"
                            + "RouterIP0={0}\n"
                            + "RouterPort0={1}\n"
                            + "IRCIP0={0}\n"
                            + "IRCPort0={2}\n"
                            + "CDKeyServerIP0={4}\n" //{0}\n"
                            + "CDKeyServerPort0={3}";
            var cfg = ServerConfig.Instance;
            initResponse = string.Format(template, cfg.HostName, cfg.RouterPort, cfg.IRCPort, cfg.CDKeyPort, cfg.CDKeyHost);
        }

        static string HandleRequest(HttpListenerRequest req)
        {
            logger.WriteDebug("InitServer            {0}Request: {1}", req.RemoteEndPoint.ToString().PadRight(22), req.RawUrl);
            return initResponse;
        }
    }
}
