import {Component} from "react";
import axios from "../services/api/axios";

export default class Track extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: props.name
        };
        this.removeFromFavourites = this.removeFromFavourites.bind(this);
        this.addToFavourites = this.addToFavourites.bind(this);
        this.delete = this.delete.bind(this);
        this.changeName = this.changeName.bind(this);
    }

    removeFromFavourites() {
        axios.post(`/users/removeTrackFromFavourites?userId=${this.props.userId}&trackId=${this.props.id}`)
            .then(res => {
                console.log(res.data);
                this.props.delete(this.props.id);
            });
    }

    addToFavourites() {
        axios.post(`/users/addTrackToFavourites?userId=${this.props.userId}&trackId=${this.props.id}`)
            .then(res => {
                console.log(res.data);
                this.props.delete(this.props.id);
            });
    }

    delete() {
        axios.post(`/tracks/removeTrack?userId=${this.props.id}`)
            .then(res => {
                if (res.status === 201) {
                    console.log(res.data);
                    this.props.delete(this.props.id);
                }
            })
    }

    changeName() {
        axios.post(`/tracks/changeName?id=${this.props.id}&name=${this.state.name}`)
            .then(res => {
                if (res.status === 201) {
                    console.log(res.data);
                }
            });
    }

    render() {
        return <div className="track">
            <p>Id: {this.props.id}</p>
            {this.props.userId != null
                ? this.props.fav
                    ? <button onClick={this.removeFromFavourites}>remove from favourites</button>
                    : <button onClick={this.addToFavourites}>add to favourites</button>
                : null}
            Name: <input type="text" value={this.state.name} onChange={e => {
            this.setState({name: e.target.value});
        }}/>
            <button onClick={this.changeName}>Change</button>
            <br/>
            <button onClick={this.delete}>delete</button>
        </div>;
    }
}