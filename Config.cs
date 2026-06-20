namespace IRCrarria

using System
using System.Collections.Generic;
using System.Linq
using Tomlyn
using Tomlyn.Model

type Config(configText: string) =
    // Parse the document immediately on instantiation
    let document = Toml.ToModel(configText)
    let hostTable = document["host"] :?> TomlTable
    let ircTable = document["irc"] :?> TomlTable

    // Expose read-only properties automatically
    member _.Hostname : string = hostTable["hostname"] :?> string
    member _.Port : int = int(hostTable["port"] :?> int64)
    member _.UseSsl : bool = hostTable["ssl"] :?> bool
    member _.SkipCertValidation : bool = hostTable["skip_cert_validation"] :?> bool
    member _.IrcLog : bool = hostTable["irc_log"] :?> bool
    
    member _.Username : string = ircTable["username"] :?> string
    member _.Nickname : string = ircTable["nickname"] :?> string
    member _.Channel : string = ircTable["channel"] :?> string
    member _.Prefix : string = ircTable["prefix"] :?> string

    // Handle optional fields safely using F# Options converted to Nullable/IEnumerable
    member _.ExtraDetails : IEnumerable<KeyValuePair<string, obj>> =
        if document.ContainsKey("server_details") then 
            document["server_details"] :?> TomlTable :> IEnumerable<KeyValuePair<string, obj>>
        else 
            null

    member _.ConnectCommands : IEnumerable<string> =
        if ircTable.ContainsKey("connect_commands") then
            (ircTable["connect_commands"] :?> TomlArray).OfType<string>()
        else 
            null
