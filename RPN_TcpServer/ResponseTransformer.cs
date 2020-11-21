namespace RPN_TcpServer
{
	public delegate TReturn ResponseTransformer<out TReturn>(string input);
};
