import {Component} from "react";
import axios from "../services/api/axios";

export default class Playlist extends Component {
    constructor(props) {
        super(props);
        this.removePlaylistFromLiked = this.removePlaylistFromLiked.bind(this);
        this.addPlaylistToLiked = this.addPlaylistToLiked.bind(this);
    }

    removePlaylistFromLiked() {
        axios.post(`/users/removePlaylistFromLiked?userId=${this.props.userId}&playlistId=${this.props.id}`)
            .then(res => {
                if (res.status === 201) {
                    console.log(res.data);
                    this.props.move(this.props.id);
                }
            });
    }

    addPlaylistToLiked() {
        axios.post(`/users/addPlaylistToLiked?userId=${this.props.userId}&playlistId=${this.props.id}`)
            .then(res => {
                if (res.status === 201) {
                    console.log(res.data);
                    this.props.move(this.props.id);
                }
            });
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