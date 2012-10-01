using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Tests.Helpers
{
	internal class TestHost : IDisposable
	{
		private readonly ServiceHost host;

		internal const string NamedPipeServiceAddress = "net.pipe://localhost/RestaurantService";
		internal const string NamedPipeMexAddress = "net.pipe://localhost/RestaurantService/mex";

		internal const string HttpServiceAddress = "http://localhost/RestaurantService";
		internal const string HttpMexAddress = "http://localhost/RestaurantService/mex";

		internal const string HttpGetAddress = "http://localhost/RestaurantService?wsdl";

		internal const string TcpServiceAddress = "net.tcp://localhost:8080/RestaurantService";
		internal const string TcpMexAddress = "net.tcp://localhost:8080/RestaurantService/mex";

		internal TestHost()
		{
			host = new ServiceHost(typeof(RestaurantService));
			ServiceMetadataBehavior behavior = new ServiceMetadataBehavior {HttpGetEnabled = true, HttpGetUrl = new Uri(HttpServiceAddress)};
			host.Description.Behaviors.Add(behavior);
		}

		internal void AddNamePipeEndpoints()
		{
			host.AddServiceEndpoint(typeof(IRestaurantService), new NetNamedPipeBinding(), NamedPipeServiceAddress);
			host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexNamedPipeBinding(), NamedPipeMexAddress);
		}

		internal void AddTcpEndpoints()
		{
			host.AddServiceEndpoint(typeof(IRestaurantService), new NetTcpBinding(), TcpServiceAddress);
			host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), TcpMexAddress);
		}

		internal void AddHttpEndpoints()
		{
			host.AddServiceEndpoint(typeof(IRestaurantService), new BasicHttpBinding(), HttpServiceAddress);
			host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), HttpMexAddress);
		}

		internal void Open()
		{
			host.Open();
		}
		
		public void Dispose()
		{
			if (host.State == CommunicationState.Faulted)
			{
				host.Abort();
			}
			else
			{
				host.Close();
			}
		}
	}
}