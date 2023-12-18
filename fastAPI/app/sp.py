import spotipy
from spotipy.oauth2 import SpotifyClientCredentials
import pandas as pd
import numpy as np
import json

credential_file = open('credentials.txt', 'r')
credentials = credential_file.readlines()
spotify_client_id = credentials[0].strip()
spotify_client_secret = credentials[1].strip()

client_credentials_manager = SpotifyClientCredentials(client_id=spotify_client_id, client_secret=spotify_client_secret)
sp = spotipy.Spotify(client_credentials_manager=client_credentials_manager)


def get_playlist_tracks(playlist_id):
    playlist = sp.playlist(playlist_id)
    tracks = playlist['tracks']
    tracks_df = pd.DataFrame(columns=['track_name', 'artist_name'])
    for i, item in enumerate(tracks['items']):
        track = item['track']
        tracks_df.loc[i] = [track['name'], track['artists'][0]['name']]
    return tracks_df

def get_audio_features(track_name, artist_name):
    results = sp.search(q='track:' + track_name + ' artist:' + artist_name, type='track')
    items = results['tracks']['items']
    if len(items) > 0:
        track = items[0]
        track_id = track['id']
        audio_features = sp.audio_features(track_id)[0]
        audio_features_df = pd.DataFrame(audio_features, index=[0])
        return audio_features_df
    else:
        return None
    


def get_playlist_audio_features(playlist_id):
    playlist_tracks = get_playlist_tracks(playlist_id)
    playlist_audio_features = list()
    for i, row in playlist_tracks.iterrows():
        audio_features = get_audio_features(row['track_name'], row['artist_name'])
        if audio_features is not None:
            playlist_audio_features.append(audio_features)
    playlist_audio_features_df = pd.concat(playlist_audio_features, ignore_index=True)
    playlist_audio_features_df = playlist_audio_features_df.drop(['type', 'id', 'uri', 'track_href', 'analysis_url','duration','key','mode'], axis=1)
    playlist_audio_features_df = playlist_audio_features_df.dropna()
    playlist_audio_features_df = playlist_audio_features_df.reset_index(drop=True)
    playlist_audio_features_df = playlist_audio_features_df.rename(columns={'duration_ms': 'duration'})

    print(playlist_audio_features_df)
    return playlist_audio_features_df

get_playlist_audio_features('37i9dQZF1DXcBWIGoYBM5M')
    