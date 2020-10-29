namespace TcpServer
{
	public delegate TReturn IResponseTransformer<TReturn>(string input);
};
