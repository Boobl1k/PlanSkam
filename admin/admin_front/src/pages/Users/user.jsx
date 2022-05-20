import {Component} from "react";
import Track from "../../components/Track"
import Playlist from "../../components/Playlist";

export default class User extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: {},
            isAuthor: false,
            tracks: [],
            playlists: [],
            availablePlaylists: []
        }
        fetch(`http://localhost:3000/users?id=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({user: JSON.parse(text)});
            }))
        fetch(`http://localhost:3000/users/isAuthor?id=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({isAuthor: text === 'true'});
            }))
        fetch(`http://localhost:3000/users/getFavTracks?id=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({tracks: JSON.parse(text)});
            }))
        fetch(`http://localhost:3000/playlists/getLikedPlaylists?userId=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({playlists: JSON.parse(text)})
            }))
        fetch(`http://localhost:3000/playlists/getAvailablePlaylists?userId=${props.id}`)
            .then(res => res.text().then(text => {
                console.log(text);
                this.setState({availablePlaylists: JSON.parse(text)})
            }))

        this.changeAuthorState = this.changeAuthorState.bind(this);
        this.changeEmail = this.changeEmail.bind(this);
        this.deleteTrack = this.deleteTrack.bind(this);
        this.removePlaylistFromLiked = this.removePlaylistFromLiked.bind(this);
        this.addPlaylistToLiked = this.addPlaylistToLiked.bind(this);
    }

    changeAuthorState() {
        const isAuthor = this.state.isAuthor;
        const req = new XMLHttpRequest();
        const method = isAuthor ? "makeNotAuthor" : "makeAuthor"
        req.open('POST', `http://localhost:3000/users/` + method + `?id=${this.state.user.Id}`);
        req.send();
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4)
                console.log(req.responseText);
            if (req.readyState === 4 && req.status === 201)
                this.setState({isAuthor: !isAuthor});
        });
    }

    changeEmail() {
        const req = new XMLHttpRequest();
        req.open('POST', `http://localhost:3000/users/changeEmail?id=${this.state.user.Id}&email=${this.state.user.Email}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4)
                console.log(req.responseText);
        });
        req.send();
    }

    deleteTrack(trackId) {
        const tracks = this.state.tracks;
        tracks.splice(this.state.tracks.indexOf(this.state.tracks.find(track => track.Id === trackId)), 1);
        this.setState({tracks: tracks});
    }
    
    removePlaylistFromLiked(playlistId){
        const playlists = this.state.playlists;
        const playlist = this.state.playlists.find(playlist => playlist.Id === playlistId);
        playlists.splice(playlists.indexOf(playlist), 1);
        this.setState({playlists: playlists});
        const available = this.state.availablePlaylists;
        available.push(playlist);
        this.setState({availablePlaylists: available});
    }

    addPlaylistToLiked(playlistId){
        const playlists = this.state.playlists;
        const playlist = this.state.availablePlaylists.find(playlist => playlist.Id === playlistId);
        playlists.push(playlist);
        this.setState({playlists: playlists});
        const available = this.state.availablePlaylists;
        available.splice(available.indexOf(playlist), 1);
        this.setState({availablePlaylists: available});
    }

    render() {
        return <div>
            <p>Id: {this.state.user.Id}</p>
            <p>UserName: {this.state.user.UserName}</p>
            <p>{this.state.isAuthor ? 'Author' : 'Not author'}</p>
            <button onClick={this.changeAuthorState}>
                {this.state.isAuthor ? 'Make not author' : 'Make author'}
            </button>
            <br/>
            <input type="text" name="email" value={this.state.user.Email} onChange={e => {
                const user = this.state.user;
                user.Email = e.target.value;
                this.setState({user});
            }}/>
            <br/>
            <button onClick={this.changeEmail}>Change email</button>
            <h1>Tracks:</h1>
            {this.state.tracks.map(track => {
                return <Track id={track.Id} name={track.Name} userId={this.state.user.Id} delete={this.deleteTrack}/>;
            })}
            <h1>Liked playlists:</h1>
            {this.state.playlists.map(playlist => {
                return <Playlist id={playlist.Id} name={playlist.Name} userId={this.state.user.Id} 
                                 isLiked={true} move={this.removePlaylistFromLiked}/>
            })}
            <h1>Available playlists:</h1>
            {this.state.availablePlaylists.map(playlist => {
                return <Playlist id={playlist.Id} name={playlist.Name} userId={this.state.user.Id} 
                                 isLiked={false} move={this.addPlaylistToLiked}/>
            })}
        </div>
    }
}