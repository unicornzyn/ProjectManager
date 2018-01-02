using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace Common
{
    /// <summary>
    /// 作者：刘玉磊
    /// 时间：2012-05-22
    /// 描述：服务操作类
    /// </summary>
    public class SerivceControl
    {

        /// <summary>   
        /// 作者：刘玉磊
        /// 时间：2012-05-22
        /// 描述：启动服务   
        /// </summary>   
        /// <param name="serviceName"></param>   
        /// <returns></returns>   
        public static bool ServiceStart(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
                else
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(1000 * 10);
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch
            {

                return false;
            }
            return true;

        }

        /// <summary>   
        /// 作者：刘玉磊
        /// 时间：2012-05-22
        /// 描述：停止服务   
        /// </summary>   
        /// <param name="serviceName"></param>   
        /// <returns></returns>  
        public static bool ServiceStop(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    return false;
                }
                else
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(1000 * 10);
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>   
        /// 服务是否存在   
        /// </summary>   
        /// <param name="serviceName"></param>   
        /// <returns></returns>   
        public static bool ServiceExist(string serviceName)
        {
            try
            {
                string m_ServiceName = serviceName;
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController s in services)
                {
                    if (s.ServiceName.ToLower() == m_ServiceName.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
