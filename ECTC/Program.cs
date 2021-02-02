using System;
using System.Collections.Generic;
using CommandLine;
using System.Linq;
using ECTC;

public class Program
{
	public static void Main(string[] args)
	{
        string av = "--from=utf-8 --to=euc-kr --path=C:\\Users\\rinechran\\NewFolder\\ECTC\\ECTC";
        var b = av.Split();
        var result = Parser.Default.ParseArguments<Options>(b)
            .WithParsed(option => {
                Ectc ectc = new Ectc(option);
                ectc.Run();
            });

    }

}

