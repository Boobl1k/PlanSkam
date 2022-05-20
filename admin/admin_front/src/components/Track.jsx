import {Component} from "react";

export default class Track extends Component {
    constructor(props) {
        super(props);
        this.removeFromFavourites = this.removeFromFavourites.bind(this);
        this.delete = this.delete.bind(this);
    }

    removeFromFavourites() {
        const req = new XMLHttpRequest();
        req.open('POST',
            `http://localhost:3000/users/removeTrackFromFavourites?userId=${this.props.userId}&trackId=${this.props.id}`);
        req.send();
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4) {
                console.log(req.responseText);
                this.props.delete(this.props.id);
            }
        });
    }
    
    delete(){
        const req = new XMLHttpRequest();
        req.open('POST',
            `http://localhost:3000/tracks/removeTrack?userId=${this.props.id}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4 && req.status === 201) {
                console.log(req.responseText);
                this.props.delete(this.props.id);
            }
        });
        req.send();
    }

    render() {
        return <div className="track">
            <p>{this.props.id} {this.props.name}</p>
            {this.props.userId != null
                ? <button onClick={this.removeFromFavourites}>remove from favourites</button>
                : null}
            <button onClick={this.delete}>delete</button>
        </div>;
    }
}