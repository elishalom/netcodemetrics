using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;

namespace CodeMetrics.Addin
{
	public class Connect : IDTExtensibility2
	{
        private DTE2 dte;
        private AddIn addInInstance;
	    private ComplexityViewManager complexityViewManager;

	    public Connect()
		{
		}

		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			dte = (DTE2)application;
			addInInstance = (AddIn)addInInst;
		}

	    public void OnStartupComplete(ref Array custom)
	    {
	        complexityViewManager = new ComplexityViewManager(dte);
	    }


	    public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
	    {
	        complexityViewManager.Dispose();
	    }

	    public void OnAddInsUpdate(ref Array custom)
		{
		}

	    

	    public void OnBeginShutdown(ref Array custom)
		{
		}
	}
}