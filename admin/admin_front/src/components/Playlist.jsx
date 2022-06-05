import {Component} from "react";
import {Button} from "react-bootstrap";
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
            <h6>
                {this.props.id} {this.props.name}
            </h6>
            <Button variant="outline-primary" onClick={this.props.isLiked ? this.removePlaylistFromLiked : this.addPlaylistToLiked}>{this.props.isLiked ? "remove" : "add"}</Button>{' '}
        </div>
    }
}