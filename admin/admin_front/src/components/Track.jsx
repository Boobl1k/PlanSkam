import {Component} from "react";

export default class Track extends Component {
    constructor(props) {
        super(props);
        this.removeFromFavourites = this.removeFromFavourites.bind(this);
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

    render() {
        return <div className="track">
            <p>{this.props.id} {this.props.name}</p>
            <button onClick={this.removeFromFavourites}>remove from favourites</button>
        </div>;
    }
}