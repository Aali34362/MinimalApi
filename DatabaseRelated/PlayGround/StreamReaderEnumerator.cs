using System.Collections;

namespace PlayGround;


// When you implement IEnumerable(T), you must also implement IEnumerator(T),
// which will walk through the contents of the file one line at a time.
// Implementing IEnumerator(T) requires that you implement IEnumerator and IDisposable.

public class StreamReaderEnumerator : IEnumerator<string>
{
    private StreamReader _sr;

    public StreamReaderEnumerator(string filePath)
    {
        _sr = new StreamReader(filePath);
    }

    private string _current;
    // Implement the IEnumerator(T).Current publicly, but implement
    // IEnumerator.Current, which is also required, privately.
    public string Current
    {

        get
        {
            if (_sr == null || _current == null)
            {
                throw new InvalidOperationException();
            }

            return _current;
        }
    }

    private object Current1
    {

        get { return this.Current; }
    }

    object IEnumerator.Current
    {
        get { return Current1; }
    }

    // Implement MoveNext and Reset, which are required by IEnumerator.
    public bool MoveNext()
    {
        _current = _sr.ReadLine();
        if (_current == null)
            return false;
        return true;
    }

    public void Reset()
    {
        _sr.DiscardBufferedData();
        _sr.BaseStream.Seek(0, SeekOrigin.Begin);
        _current = null;
    }

    private bool disposedValue = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // Dispose of managed resources.
            }
            _current = null;
            if (_sr != null)
            {
                _sr.Close();
                _sr.Dispose();
            }
        }

        this.disposedValue = true;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    ~StreamReaderEnumerator()
    {
        Dispose(disposing: false);
    }
}
