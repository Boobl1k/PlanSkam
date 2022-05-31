import {Component} from "react";
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
                <button onClick={this.changeName}>
                    Change name
                </button>
                <NavLink to={`../author/${this.props.id}`}>Open</NavLink>
            </div>
        );
    }
}