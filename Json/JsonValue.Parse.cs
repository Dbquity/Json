using System;

namespace Dbquity {
    using static JsonValue.Tokens;
    partial class JsonValue {
        internal enum Tokens { BeginObject, EndObject, BeginArray, EndArray, Colon, Comma, Text, True, False, Null, Number };
        private static (JsonValue value, int index) Parse(string text, int index) {
            int beginLiteral = 0, endLiteral = 0;
            return Process(GetNextToken());

            Tokens GetNextToken()
            {
                char c;
                do {
                    if (index >= text.Length)
                        throw NewAE("Unexpected end of text");
                    c = text[index++];
                } while (char.IsWhiteSpace(c));
                switch (c) {
                    case '{': return BeginObject;
                    case '}': return EndObject;
                    case '[': return BeginArray;
                    case ']': return EndArray;
                    case '"':
                        beginLiteral = endLiteral = index;
                        while (index < text.Length) {
                            c = text[index++];
                            if (c == '\\') {
                                c = text[index++];
                                endLiteral++;
                            } else if (c == '\"')
                                return Tokens.Text;
                            endLiteral++;
                        }
                        throw NewAE("Missing end quote '\"'");
                    case ':': return Colon;
                    case ',': return Comma;
                    case 't':
                        if (!text.Substring(index).StartsWith("rue"))
                            throw NewAE($"Unknown token t{text.Substring(index).Split(' ', '\t', '\r', '\n')[0]}");
                        index += 3;
                        return True;
                    case 'f':
                        if (!text.Substring(index).StartsWith("alse"))
                            throw NewAE($"Unknown token f{text.Substring(index).Split(' ', '\t', '\r', '\n')[0]}");
                        index += 4;
                        return False;
                    case 'n':
                        if (!text.Substring(index).StartsWith("ull"))
                            throw NewAE($"Unknown token n{text.Substring(index).Split(' ', '\t', '\r', '\n')[0]}");
                        index += 3;
                        return Tokens.Null;
                    case '-':
                    case '+':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        beginLiteral = endLiteral = index - 1;
                        if (c == '-' || c == '+')
                            TakeOne(aDigitMustFollow: true);
                        while (char.IsDigit(c))
                            TakeOne();
                        if (c == '.')
                            TakeOne(aDigitMustFollow: true);
                        while (char.IsDigit(c))
                            TakeOne();
                        if (c == 'e' || c == 'E') {
                            TakeOne();
                            if (c == '-' || c == '+')
                                TakeOne(aDigitMustFollow: true);
                            else
                                if (!char.IsDigit(c))
                                    throw NewAE($"Expected a digit but got '{c}'");
                            while (char.IsDigit(c))
                                TakeOne();
                        }
                        index--;
                        return Tokens.Number;
                }
                throw NewAE($"Unexpected character '{c}'");
                void TakeOne(bool aDigitMustFollow = false)
                {
                    endLiteral++;
                    c = index < text.Length ? text[index++] : (char)0;
                    if (aDigitMustFollow && !char.IsDigit(c))
                        throw NewAE($"Expected a digit but got '{c}'");
                }
            }
            (JsonValue, int) Process(Tokens t)
            {
                switch (t) {
                    case Tokens.Null:
                        return (JsonNull.Instance, index);
                    case True:
                        return (new JsonBool { Value = true }, index);
                    case False:
                        return (new JsonBool { Value = false }, index);
                    case Tokens.Number:
                        return (new JsonNumber { String = text.Substring(beginLiteral, endLiteral - beginLiteral) }, index);
                    case Tokens.Text:
                        return (new JsonText { Value = text.Substring(beginLiteral, endLiteral - beginLiteral) }, index);
                    case BeginObject:
                        JsonObject o = new JsonObject();
                        do {
                            switch (t = GetNextToken()) {
                                case EndObject:
                                    return (o, index);
                                case Tokens.Text:
                                    string propertyName = text.Substring(beginLiteral, endLiteral - beginLiteral);
                                    if (GetNextToken() != Colon)
                                        throw NewAE("':' expected");
                                    JsonValue propertyValue;
                                    (propertyValue, index) = Parse(text, index);
                                    o.Properties.Add(propertyName, propertyValue);
                                    break;
                                case Comma:
                                    break;
                                default:
                                    throw NewAE($"Unexpected token {t}");
                            }
                        } while (true);
                    case BeginArray:
                        JsonArray a = new JsonArray();
                        do {
                            switch (t = GetNextToken()) {
                                case EndArray:
                                    return (a, index);
                                case Comma:
                                    break;
                                default:
                                    JsonValue elementValue;
                                    (elementValue, index) = Process(t);
                                    a.Items.Add(elementValue);
                                    break;
                            }
                        } while (true);
                    default:
                        throw NewAE($"Unexpected token {t}");
                }
            }
            ArgumentException NewAE(string message) {
                int start = Math.Max(0, index - 30);
                return new ArgumentException($"{message} at position {index} around '{text.Substring(start, Math.Min(text.Length - start, 50))}'");
            }
        }
    }
}
