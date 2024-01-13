﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.Kusto.ServiceLayer.DataSource.Intellisense;
using Microsoft.Kusto.ServiceLayer.Workspace.Contracts;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlTools.Utility;

namespace Microsoft.Kusto.ServiceLayer.LanguageServices
{
    /// <summary>
    /// A class to calculate the numbers used by SQL parser using the text positions and content
    /// </summary>
    public class ScriptDocumentInfo
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public ScriptDocumentInfo(TextDocumentPosition textDocumentPosition, ScriptFile scriptFile, ScriptParseInfo scriptParseInfo)
            : this(textDocumentPosition, scriptFile)
        {
            Validate.IsNotNull(nameof(scriptParseInfo), scriptParseInfo);

            ScriptParseInfo = scriptParseInfo;
        }

        private ScriptDocumentInfo(TextDocumentPosition textDocumentPosition, ScriptFile scriptFile)
        {
            StartLine = textDocumentPosition.Position.Line;
            ParserLine = textDocumentPosition.Position.Line + 1;
            StartColumn = TextUtilities.PositionOfPrevDelimeter(
                                scriptFile.Contents,
                                textDocumentPosition.Position.Line,
                                textDocumentPosition.Position.Character);
            EndColumn = TextUtilities.PositionOfNextDelimeter(
                                scriptFile.Contents,
                                textDocumentPosition.Position.Line,
                                textDocumentPosition.Position.Character);
            ParserColumn = textDocumentPosition.Position.Character + 1;
            Contents = scriptFile.Contents;
        }

        /// <summary>
        /// Creates a new <see cref="ScriptDocumentInfo"/> with no backing <see cref="ScriptParseInfo"/> defined
        /// </summary>
        /// <param name="textDocumentPosition">A <see cref="TextDocumentPosition"/></param>
        /// <param name="scriptFile">A <see cref="ScriptFile"/> to process</param>
        /// <returns></returns>
        public static ScriptDocumentInfo CreateDefaultDocumentInfo(TextDocumentPosition textDocumentPosition, ScriptFile scriptFile)
        {
            return new ScriptDocumentInfo(textDocumentPosition, scriptFile);
        }

        /// <summary>
        /// Gets a string containing the full contents of the file.
        /// </summary>
        public string Contents { get; private set; }

        /// <summary>
        /// Script Parse Info Instance
        /// </summary>
        public ScriptParseInfo ScriptParseInfo { get; private set; }

        /// <summary>
        /// Start Line
        /// </summary>
        public int StartLine { get; private set; }

        /// <summary>
        /// Parser Line
        /// </summary>
        public int ParserLine { get; private set; }

        /// <summary>
        /// Start Column
        /// </summary>
        public int StartColumn { get; private set; }

        /// <summary>
        /// end Column
        /// </summary>
        public int EndColumn { get; private set; }

        /// <summary>
        /// Parser Column
        /// </summary>
        public int ParserColumn { get; private set; }

        /// <summary>
        /// The token text in the file content used for completion list
        /// </summary>
        public virtual string TokenText
        {
            get
            {
                return Token != null ? Token.Text : null;
            }
        }

        /// <summary>
        /// The token in the file content used for completion list
        /// </summary>
        public Token Token { get; private set; }
    }
}
