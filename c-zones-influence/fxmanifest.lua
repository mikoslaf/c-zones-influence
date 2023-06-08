fx_version 'cerulean'
game 'gta5'

mono_rt2 'Prerelease expiring 2023-06-30. See https://aka.cfx.re/mono-rt2-preview for info.'

ui_page 'nui/index.html'

files {
    'nui/index.html',
    'nui/zones.js',
    'nui/mapa2.jpeg',
    'nui/code.jquery.com_jquery-3.7.0.min.js'
}

client_scripts {
    'client/CitizenFX.Core.dll',
    'client/CitizenFX.FiveM.dll',
    'client/CitizenFX.FiveM.Native.dll',
    'client/c-zones-influence.client.net.dll'
}

server_scripts {
    'server/CitizenFX.Server.dll',
    'server/CitizenFX.Core.dll',
    'server/c-zones-influence.server.net.dll'
}