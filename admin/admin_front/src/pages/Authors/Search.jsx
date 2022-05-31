import {Component} from "react";
import Author from "../../components/Author";
import axios from "../../services/api/axios";
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

export default class Search extends Component {
    constructor(props) {
        super(props);
        this.state = {
            query: "",
            authors: []
        }
        this.search = this.search.bind(this);
    }

    search() {
        if (this.state.query !== '')
            axios.get(`/authors/search?query=${this.state.query}`)
                .then(res => {
                    console.log(res.data);
                    this.setState({authors: res.data});
                })
    }

    render() {
        return <div>
            <input type="text" value={this.state.query} onChange={e => {
                this.setState({query: e.target.value});
            }}/>
            <Button variant="outline-light" onClick={this.search}>search</Button>{' '}
            {this.state.authors.map(author => {
                return <Author id={author.Id} name={author.Name}/>
            })}
        </div>
    }
}