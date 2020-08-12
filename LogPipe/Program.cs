using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogPipe {
    class Program {
        const string InputFileArgument = "-in";
        const string OutputFileArgument = "-out";
        const string PatternArgument = "-pattern";

        static void Main(string[] args) {
            new Program(args);
        }

        public Dictionary<string, string> Arguments;

        public Program(string[] args) {
            try {
                Arguments = ParseArguments(args);

                //foreach(var key in Arguments.Keys) {
                //    Console.WriteLine($"{key} = {Arguments[key]}");
                //}

                var validArguments = true;
                foreach(var argument in new List<string> { InputFileArgument, PatternArgument }) {
                    if (!Arguments.ContainsKey(argument)) {
                        Console.WriteLine($"No { argument} argument");
                        validArguments = false;
                    }
                }

                if(!validArguments){
                    return;
                }

                if (!File.Exists(Arguments[InputFileArgument])) {
                    Console.WriteLine($"File {Arguments[InputFileArgument]} does not exists");
                    return;
                }

                if (!Arguments.ContainsKey(OutputFileArgument)) {
                    Arguments[OutputFileArgument] = GetDefaultOutputFile(Arguments[InputFileArgument]);
                }

                ExportLines(Arguments[InputFileArgument], Arguments[OutputFileArgument], Arguments[PatternArgument]);
            }catch(Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public string GetDefaultOutputFile(string inputFileName) {
            var fileInfo = new FileInfo(inputFileName);

            return $"{fileInfo.Name.Replace(fileInfo.Extension, string.Empty)}-out{fileInfo.Extension}";
        }

        public Dictionary<string, string> ParseArguments(string[] args) {
            var result = new Dictionary<string, string>();

            for(int i = 0; i < (int)Math.Floor((float)args.Length / 2); i++) {
                result.Add(args[i * 2], args[i * 2 +1]);
            }

            return result;
        }

        public void ExportLines(string inputFile, string outputFile, string pattern) {
            var inputLines = File.ReadAllLines(inputFile);

            var outputWriter = new StreamWriter(outputFile, false, Encoding.UTF8);

            foreach(var line in inputLines) {
                if (line.Contains(pattern)) {
                    outputWriter.WriteLine(line);
                }
            }

            outputWriter.Flush();
            outputWriter.Close();
        }
    }
}
