using System;
using System.IO;

public class FileLogger : IDisposable {

	private static StreamWriter logFile;
	private static readonly Object mutex = new Object();

	public static void Write(string str) {
		
		lock (mutex) {
			if (logFile == null) {
				logFile = File.CreateText("conway_cann_log.txt");
			}
			logFile.Write(str);
		}
	}

	public static void WriteLine(string str) {
		Write(str + "\n");
	}

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects).
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
			logFile.Close();

            // set large fields to null.

            disposedValue = true;
        }
    }

    // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
	~FileLogger() {
	  // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
	  Dispose(false);
	}

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // uncomment the following line if the finalizer is overridden above.
        GC.SuppressFinalize(this);
    }
    #endregion


}
