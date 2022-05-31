import {Component, Input} from "react";
import {Button, InputGroup, FormControl} from "react-bootstrap";

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
            <h6>Id: {this.props.id}</h6>
            {this.props.userId != null ? 
                this.props.fav ? 
                    <Button variant="outline-light" onClick={this.removeFromFavourites}>remove from favourites</Button> : 
                    <Button variant="outline-light" onClick={this.addToFavourites}>add to favourites</Button> : 
                null}
            Name: <InputGroup className="mb-3" type="text" value={this.state.name} onChange={e => {
            this.setState({name: e.target.value});}}>
            <FormControl
                placeholder="Recipient's username"
                aria-label="Recipient's username"
                aria-describedby="basic-addon2"
            />
        </InputGroup>
            <Button variant="outline-light" onClick={this.changeName}>Change</Button>{' '}
            <br/>
            <Button variant="outline-light" onClick={this.delete}>delete</Button>{' '}
        </div>;
    }
}