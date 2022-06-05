import {Component} from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import {NavLink} from "react-router-dom";
import axios from "../services/api/axios";

export default class Author extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: props.name
        }
        this.changeName = this.changeName.bind(this);
    }

    changeName(){
        axios.post(`/authors/changeName?id=${this.props.id}&name=${this.state.name}`)
            .then(res =>{
                console.log(res.data);
            });
    }
    
    render() {
        return (
            <div className="author">
                {this.props.id}
                <br/>
                Name: <input type="text" value={this.state.name} onChange={e => {
                this.setState({name: e.target.value});
            }}/>
                <Button variant="outline-primary" onClick={this.changeName}>Change name</Button>{' '}
                <NavLink to={`../author/${this.props.id}`}>Open</NavLink>
            </div>
        );
    }
}