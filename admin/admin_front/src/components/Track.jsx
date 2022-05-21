import {Component} from "react";

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
    
    addToFavourites(){
        const req = new XMLHttpRequest();
        req.open('POST',
            `http://localhost:3000/users/addTrackToFavourites?userId=${this.props.userId}&trackId=${this.props.id}`);
        req.send();
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4) {
                console.log(req.responseText);
                this.props.delete(this.props.id);
            }
        });
    }

    delete() {
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

    changeName() {
        const req = new XMLHttpRequest();
        req.open('POST',
            `http://localhost:3000/tracks/changeName?id=${this.props.id}&name=${this.state.name}`);
        req.addEventListener('readystatechange', () => {
            if (req.readyState === 4 && req.status === 201) {
                console.log(req.responseText);
            }
        });
        req.send();
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