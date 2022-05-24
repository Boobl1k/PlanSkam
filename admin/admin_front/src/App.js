import {
    AppBar,
    Toolbar,
    Typography,
    Button,
    makeStyles,
} from "@material-ui/core";
import {Link} from "react-router-dom";
import "./App.css";
import Routes from "./routes/Routes";

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
    },
    rightToolbar: {
        flexGrow: 1,
    },
    title: {
        marginRight: theme.spacing(2),
    },
}));

function App() {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6" className={classes.title}>
                        Admin Panel
                    </Typography>
                    <div className={classes.rightToolbar}>
                        <Button variant="h4" color="inherit" component={Link} to="/">
                            Home
                        </Button>
                        <Button variant="h4" color="inherit" component={Link} to="/users">
                            All users
                        </Button>
                        <Button variant="h4" color="inherit" component={Link} to="/tracks/search">
                            Tracks
                        </Button>
                        <Button variant="h" color="inherit" component={Link} to="authors/search">
                            Authors
                        </Button>
                    </div>
                </Toolbar>
            </AppBar>
            <Routes/>
        </div>
    );
}

export default App;
