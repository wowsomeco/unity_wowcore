using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Wowsome {
  namespace Serialization {
    /// <summary>
    /// CJson
    /// </summary>
    /// <description>
    /// uses for deserializing a json string to dictionary of string key(s) and value(s)
    /// the format must be like a common json where consists of KVP 
    /// with colon as a separator in between a key and value
    /// </description>
    public class CJson {
      StringReader m_jsonReader;

      readonly char Quotes = '"';
      readonly char KvSeparator = ':';

      char NextChar {
        get { return Convert.ToChar(m_jsonReader.Read()); }
      }

      char PeekChar {
        get { return Convert.ToChar(m_jsonReader.Peek()); }
      }

      public CJson() { }

      public Dictionary<string, string> Deserialize(string json) {
        Dictionary<string, string> result = new Dictionary<string, string>();
        // instantiate the reader
        m_jsonReader = new StringReader(json);

        while (true) {
          // keep reading
          m_jsonReader.Read();
          // until it reaches the end
          if (m_jsonReader.Peek() != -1) {
            // if a new quote is found
            if (PeekChar == Quotes) {
              // ditch the quote
              m_jsonReader.Read();
              // instantiate the key builder
              StringBuilder keyBuilder = new StringBuilder();
              // loop until 
              while (PeekChar != Quotes) {
                keyBuilder.Append(PeekChar);
                // next char
                m_jsonReader.Read();
              }
              // ditch the last quote for the key	
              m_jsonReader.Read();
              // eat the white space after the quote, if any
              EatWhitespace();
              // check whether the next char is colon
              if (NextChar == KvSeparator) {
                // eat the whitespace after the colon, if any
                EatWhitespace();
                // ditch the quote
                m_jsonReader.Read();
                // create a value builder
                StringBuilder valueBuilder = new StringBuilder();
                // loop until
                while (PeekChar != Quotes) {
                  valueBuilder.Append(PeekChar);
                  // next char
                  m_jsonReader.Read();
                }
                // add kvp to the dict
                result.Add(keyBuilder.ToString(), valueBuilder.ToString());
              }
            }
          } else {
            // break the loop when reached the end of the string
            break;
          }
        }
        // dispose the reader
        m_jsonReader.Dispose();
        // return the dict result
        return result;
      }

      void EatWhitespace() {
        while (Char.IsWhiteSpace(PeekChar)) {
          m_jsonReader.Read();
          if (m_jsonReader.Peek() == -1) {
            break;
          }
        }
      }
    }
  }
}