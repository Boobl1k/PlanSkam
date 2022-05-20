import {Component} from "react";

export default class Playlist extends Component {
    constructor(props) {
        super(props);
        this.removePlaylistFromLiked = this.removePlaylistFromLiked.bind(this);
        this.addPlaylistToLiked = this.addPlaylistToLiked.bind(this);
    }

    removePlaylistFromLiked() {
        const req = new XMLHttpRequest();
        req.open('POST',
            `http://localhost:3000/users/removePlaylistFromLiked?userId=${this.props.userId}&playlistId=${this.props.id}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4 && req.status === 201) {
                console.log(req.responseText);
                this.props.move(this.props.id);
            }
        });
        req.send();
    }

    addPlaylistToLiked() {
        const req = new XMLHttpRequest();
        req.open('POST',
            `http://localhost:3000/users/addPlaylistToLiked?userId=${this.props.userId}&playlistId=${this.props.id}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4 && req.status === 201) {
                console.log(req.responseText);
                this.props.move(this.props.id);
            }
        });
        req.send();
    }

    render() {
        return <div className="playlist">
            <p>{this.props.id} {this.props.name}</p>
            <button onClick={this.props.isLiked ? this.removePlaylistFromLiked : this.addPlaylistToLiked}>
                {this.props.isLiked ? "remove" : "add"}
            </button>
        </div>
    }
}